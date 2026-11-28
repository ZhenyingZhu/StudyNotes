# Native source-file examples

[Source manifest](../../sources.conf) | [Source setup](../DEVELOPMENT.md#choose-the-package-sources-for-your-environment)

`sources.conf` is the checked-in active manifest, not an example. This folder
contains public-only examples of the native configuration files it references:

| File | Native consumer |
|---|---|
| `NuGet.Config` | NuGet application restore and EF-tool installation |
| `.npmrc` | npm/pnpm registry configuration |

Copy missing files to `D:\ItemOrganizerConfig`, configure your approved sources,
and set their absolute paths in the project manifest. Do not overwrite files
you have already edited. Ubuntu users supply absolute Linux paths instead.
Only NuGet and npm configuration files are referenced by path. Configure apt,
signing-key, registry, installer and archive sources directly in `sources.conf`.
The installer/build generates native apt configuration from those values.

Examples contain no credentials and are never runtime fallbacks. Keep actual
native files, private feed details and credentials outside Git. If a native
file contains credentials, protect it using the applicable filesystem access
controls and supported native authentication mechanism.

Startup/installer integration with this revised manifest is pending.
