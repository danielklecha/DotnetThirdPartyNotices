﻿using DotnetThirdPartyNotices.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DotnetThirdPartyNotices.Services;

internal class NetFrameworkLicenseResolver : ILicenseUriLicenseResolver, IFileVersionInfoLicenseResolver
{
    private static string? _licenseContent;

    public Task<bool> CanResolveAsync(Uri licenseUri, ResolverOptions resolverOptions, CancellationToken cancellationToken) => Task.FromResult(licenseUri.ToString().Contains("LinkId=529443"));
    public Task<bool> CanResolveAsync(FileVersionInfo fileVersionInfo, CancellationToken cancellationToken) => Task.FromResult(fileVersionInfo.ProductName == "Microsoft® .NET Framework");

    public Task<string?> ResolveAsync(Uri licenseUri, ResolverOptions resolverOptions, CancellationToken cancellationToken) => GetLicenseContent();
    public Task<string?> ResolveAsync(FileVersionInfo fileVersionInfo, ResolverOptions resolverOptions, CancellationToken cancellationToken) => GetLicenseContent();

    public async Task<string?> GetLicenseContent()
    {
        if (_licenseContent != null) // small optimization: avoid getting the resource on every call
            return _licenseContent;
        var executingAssembly = Assembly.GetExecutingAssembly();
        await using var stream = executingAssembly.GetManifestResourceStream(typeof(Program), "dotnet_library_license.txt");
        if (stream == null)
            return null;
        using var streamReader = new StreamReader(stream);
        _licenseContent = await streamReader.ReadToEndAsync();
        return _licenseContent;
    }
}