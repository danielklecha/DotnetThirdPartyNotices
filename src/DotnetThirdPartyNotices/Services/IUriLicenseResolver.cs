﻿using System;
using System.Threading.Tasks;

namespace DotnetThirdPartyNotices.Services;

internal interface IUriLicenseResolver
{
    bool CanResolve(Uri licenseUri);
    Task<string> Resolve(Uri licenseUri);
}
