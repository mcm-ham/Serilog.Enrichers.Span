namespace Serilog.Enrichers.Span;

using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// A log event enricher which adds trace flags from the current <see cref="Activity"/>.
/// </summary>
public class ActivityTraceFlagsEnricher : ILogEventEnricher
{
    private const string TraceFlags = "TraceFlags";

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(logEvent);
#else
        if (logEvent is null)
        {
            throw new ArgumentNullException(nameof(logEvent));
        }
#endif

        var activity = Activity.Current;
        if (activity is not null)
        {
            var traceFlags = activity.ActivityTraceFlags;
            logEvent.AddPropertyIfAbsent(new LogEventProperty(TraceFlags, new ScalarValue(traceFlags)));
        }
    }
}
