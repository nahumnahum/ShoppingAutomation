using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ShoppingAutomation.Api.Automation
{
    public class StepLog
    {
        public string StepName    { get; set; } = "";
        public bool   Success     { get; set; }
        public long   DurationMs  { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AutomationLogger
    {
        public string RequestId { get; } = Guid.NewGuid().ToString("N")[..8];
        public List<StepLog> Steps { get; } = new();

        private Stopwatch? _stepTimer;
        private string?    _currentStep;

        public void StartStep(string stepName)
        {
            _currentStep = stepName;
            _stepTimer   = Stopwatch.StartNew();
            Console.WriteLine($"[{RequestId}] ▶ {stepName}");
        }

        public void EndStep(bool success = true, string? error = null)
        {
            _stepTimer?.Stop();
            var log = new StepLog
            {
                StepName    = _currentStep ?? "unknown",
                Success     = success,
                DurationMs  = _stepTimer?.ElapsedMilliseconds ?? 0,
                ErrorMessage = error,
                Timestamp   = DateTime.UtcNow
            };
            Steps.Add(log);

            if (success)
                Console.WriteLine($"[{RequestId}] ✔ {log.StepName} ({log.DurationMs}ms)");
            else
                Console.WriteLine($"[{RequestId}] ✘ {log.StepName} FAILED ({log.DurationMs}ms): {error}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"[{RequestId}] ℹ {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[{RequestId}] ⚠ {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[{RequestId}] ✘ {message}");
        }
    }
}