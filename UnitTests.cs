using System;
using System.Collections.Generic;
using System.IO;
using GameLibraryManager.Controller;
using GameLibraryManager.Data;
using GameLibraryManager.Model;
using GameLibraryManager.Logger;
using GameLibraryManager.Utilities;

namespace GameLibraryManager
{
    public class UnitTestResult
    {
    public string Name { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Message { get; set; } = string.Empty;
    }

    public static class InternalUnitTests
    {
        private static readonly ILogger _logger = FileLogger.Default;

        public static List<UnitTestResult> RunAll()
        {
            var results = new List<UnitTestResult>();

            // Backup real data
            string dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "players.json");
            string? backup = File.Exists(dataFile) ? File.ReadAllText(dataFile) : null;

            try
            {
                // Reset repo to clean state
                PlayerRepository.ResetForTesting("players_test.json", "actions_test.txt");
                var controller = new PlayerController();

                results.Add(TestAddPlayer(controller));
                results.Add(TestPreventDuplicateId(controller));
                results.Add(TestUpdateStats(controller));
                results.Add(TestLinearSearch(controller));
                results.Add(TestInsertionSort(controller));
            }
            catch (Exception ex)
            {
                results.Add(new UnitTestResult
                {
                    Name = "TestHarness",
                    Passed = false,
                    Message = "Exception in test harness: " + ex.Message
                });
            }
            finally
            {
                // Restore real data
                try
                {
                    if (backup != null)
                        File.WriteAllText(dataFile, backup);
                    else if (File.Exists(dataFile))
                        File.Delete(dataFile);

                    // Clean up test files
                    var testFiles = new[] { "players_test.json", "actions_test.txt" };
                    foreach (var f in testFiles)
                    {
                        if (File.Exists(f)) File.Delete(f);
                    }
                }
                catch { /* ignore cleanup errors */ }
            }

            // Log results
            foreach (var r in results)
            {
                _logger.Log($"UNIT TEST {r.Name} - {(r.Passed ? "PASS" : "FAIL")} - {r.Message}");
            }

            return results;
        }

        // 1. Add Player
        private static UnitTestResult TestAddPlayer(PlayerController ctrl)
        {
            try
            {
                ctrl.AddPlayer(1001, "test_user");
                var p = ctrl.GetById(1001);
                bool pass = p != null && p.Username == "test_user";
                return new UnitTestResult
                {
                    Name = "TestAddPlayer",
                    Passed = pass,
                    Message = pass ? "Player added and retrieved." : "Failed to add or find player."
                };
            }
            catch (Exception ex)
            {
                return new UnitTestResult { Name = "TestAddPlayer", Passed = false, Message = ex.Message };
            }
        }

        // 2. Prevent Duplicate ID
        private static UnitTestResult TestPreventDuplicateId(PlayerController ctrl)
        {
            try
            {
                ctrl.AddPlayer(2002, "dup1");
                bool threw = false;
                try
                {
                    ctrl.AddPlayer(2002, "dup2"); // should fail
                }
                catch
                {
                    threw = true;
                }
                return new UnitTestResult
                {
                    Name = "TestPreventDuplicateId",
                    Passed = threw,
                    Message = threw ? "Duplicate ID correctly blocked." : "Allowed duplicate ID (FAIL)."
                };
            }
            catch (Exception ex)
            {
                return new UnitTestResult { Name = "TestPreventDuplicateId", Passed = false, Message = ex.Message };
            }
        }

        // 3. Update Stats
        private static UnitTestResult TestUpdateStats(PlayerController ctrl)
        {
            try
            {
                ctrl.AddPlayer(3003, "updater");
                ctrl.UpdatePlayerStats(3003, 2.5, 800);
                var p = ctrl.GetById(3003);
                bool pass = p != null && Math.Abs(p.HoursPlayed - 2.5) < 0.001 && p.HighScore == 800;
                return new UnitTestResult
                {
                    Name = "TestUpdateStats",
                    Passed = pass,
                    Message = pass ? "Stats updated correctly." : "Stats not updated as expected."
                };
            }
            catch (Exception ex)
            {
                return new UnitTestResult { Name = "TestUpdateStats", Passed = false, Message = ex.Message };
            }
        }

        // 4. Linear Search
        private static UnitTestResult TestLinearSearch(PlayerController ctrl)
        {
            try
            {
                ctrl.AddPlayer(4004, "SEARCH_ME");
                var byId = ctrl.GetById(4004);
                var byName = ctrl.GetByUsername("search_me"); // case-insensitive
                bool pass = byId != null && byName != null && byId.Id == byName.Id;
                return new UnitTestResult
                {
                    Name = "TestLinearSearch",
                    Passed = pass,
                    Message = pass ? "Linear search works (case-insensitive)." : "Search failed."
                };
            }
            catch (Exception ex)
            {
                return new UnitTestResult { Name = "TestLinearSearch", Passed = false, Message = ex.Message };
            }
        }

        // 5. Insertion Sort
        private static UnitTestResult TestInsertionSort(PlayerController ctrl)
        {
            try
            {
                ctrl.AddPlayer(3, "z");
                ctrl.AddPlayer(1, "x");
                ctrl.AddPlayer(2, "y");
                ctrl.SetSortStrategy(new InsertionSortStrategy());
                var sorted = ctrl.Sort(PlayerSortField.Id, false);
                bool isSorted = sorted.Count == 3 &&
                                sorted[0].Id == 1 &&
                                sorted[1].Id == 2 &&
                                sorted[2].Id == 3;
                return new UnitTestResult
                {
                    Name = "TestInsertionSort",
                    Passed = isSorted,
                    Message = isSorted ? "Insertion sort works." : "Insertion sort failed."
                };
            }
            catch (Exception ex)
            {
                return new UnitTestResult { Name = "TestInsertionSort", Passed = false, Message = ex.Message };
            }
        }
    }
}