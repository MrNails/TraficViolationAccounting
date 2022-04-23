using System;
using System.Collections.Generic;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;

namespace AccountingOfTrafficViolation.Services;

public static class GlobalSettings
{
    public static readonly Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>();
    public static readonly int SaltSize = 16;

    private static Officer m_activeOfficer;
    private static ILogger m_logger;
    private static TVAContext? m_globalContext;

    public static Officer ActiveOfficer
    {
        get => m_activeOfficer;
        set => m_activeOfficer = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public static ILogger Logger
    {
        get => m_logger;
        set => m_logger = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static TVAContext? GlobalContext
    {
        get => m_globalContext;
        set => m_globalContext = value ?? throw new ArgumentNullException(nameof(value));
    }
}