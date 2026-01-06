/*
 * SnakeUSBIP Server - Security Helpers
 * (c) 2025 SnakeFoxu - Protocolo Omega Compliant
 * https://github.com/SnakeFoxu/SnakeUSBIP-Server
 */

using System.Text.RegularExpressions;

namespace SnakeUSBIP.Server.Services;

/// <summary>
/// Security utilities for input validation and sanitization.
/// </summary>
public static class SecurityHelper
{
    /// <summary>
    /// Validates a Bus ID format (e.g., "1-2", "1-2.3").
    /// </summary>
    public static bool IsValidBusId(string? busId)
    {
        if (string.IsNullOrWhiteSpace(busId)) return false;

        // BusId format: digit(s)-digit(s), optionally followed by .digit(s)
        // Examples: "1-2", "1-2.3", "2-1.4.5"
        return Regex.IsMatch(busId, @"^\d+-[\d.]+$");
    }

    /// <summary>
    /// Sanitizes a command argument to prevent injection attacks.
    /// Only allows alphanumeric, hyphens, dots, and underscores.
    /// </summary>
    public static string SanitizeArgument(string? arg)
    {
        if (string.IsNullOrWhiteSpace(arg)) return string.Empty;

        // Remove any characters that are not safe for command line arguments
        return Regex.Replace(arg, @"[^a-zA-Z0-9\-._]", "");
    }

    /// <summary>
    /// Validates that a path is safe and doesn't contain path traversal attempts.
    /// </summary>
    public static bool IsSafePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;

        // Check for path traversal attempts
        if (path.Contains("..") || path.Contains("//") || path.Contains("\\\\"))
            return false;

        // Check for command injection attempts
        if (path.Contains(';') || path.Contains('&') || path.Contains('|') || 
            path.Contains('$') || path.Contains('`'))
            return false;

        return true;
    }
}
