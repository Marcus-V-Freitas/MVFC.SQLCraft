# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.1.1] - 2026-04-05

### Added

- Automated tag and release rollback in CI/CD pipeline on job failure to ensure repository consistency.

### Changed

- Modernized NuGet package icons across all projects for a more refined visual identity.

## [3.1.0] - 2026-04-01

### Added

- Integrated `MinVer` for automated semantic versioning based on git tags
- Centralized common project properties (`LangVersion`, `Nullable`, `ImplicitUsings`) and analysis configurations in `Directory.Build.props`

### Changed

- Simplified CI/CD workflow (`ci.yml`) by delegating package versioning entirely to MinVer during `dotnet pack`
- Required full git history checkout (`fetch-depth: 0`) in GitHub Actions for accurate version resolution
- Cleansed redundant configurations and metadata from individual `.csproj` files
- Renamed `Directory.Build.target` to `Directory.Build.targets` for appropriate MSBuild recognition
- Excluded test and playground projects from code coverage extraction (`codecov.yml` and `coverage.runsettings`)

### Fixed

- Made `CustomTestQueryFactory` implement `IDisposable` to prevent resource leakage

## [3.0.9] - 2026-03-21

### Changed
- CI/CD workflow refinements for automated publishing and coverage reporting
- Minor adjustments to Codecov configuration for status checks precision

## [3.0.8] - 2026-03-14

### Added

- Standardized project badges (CI, CodeCov, License, Platform, NuGet) across all internal README files (EN and PT-BR).

### Changed

- Enhanced CI workflow:
  - Removed redundant .NET 9 setup (now using .NET 10 consistently).
  - Added repository-relative "What's Changed" link to automated GitHub Release body.
- Updated code coverage filtering to exclude `Playground`, `AppHost`, and `Tests` assemblies from collection and reporting.

## [3.0.7] - 2026-03-14

### Added

- Multi-language support for all project documentation:
  - English translations for all internal package READMEs.
  - Portuguese versions (`README.pt-br.md`) for all internal packages.
  - Quick-switch language links at the top of all documentation files.
- Automated GitHub Release creation in CI workflow using `softprops/action-gh-release`.
- Self-installing build tools in Cake script (`dotnet-coverage`, `ReportGenerator`).

### Changed

- Refactored `build.cake` to use `dotnet-coverage` for improved code coverage collection (supporting multi-process/Aspire).
- Improved build script resilience by adding exit code validation for all internal processes.

## [3.0.6] - 2026-03-12

### Changed

- Improved dotnet-cake build script

## [3.0.5] - 2026-03-12

### Changed

- Unified CI and CD into a single workflow in `.github/workflows/ci.yml`
- Added automated publishing to NuGet and GitHub Packages for tagged releases

### Removed

- Removed redundant `.github/workflows/publish.yml` workflow

## [3.0.4] - 2026-03-12

### Changed

- Added `main` branch trigger to CI workflow in `.github/workflows/ci.yml`
- Configured default branch to `main` in `codecov.yml`

## [3.0.3] - 2026-03-11

### Changed

- Added NuGet download badges to the packages table in `README.md` and `README.pt-BR.md`

## [3.0.2] - 2026-03-11

### Removed

- Removed `tools/` directory (report generator binaries) and stopped tracking test result folders

## [3.0.1] - 2026-03-11

### Changed

- Updated `publish.yml` workflow (fixed folder name `test` to `tests`)

## [3.0.0] - 2026-03-11

### Added

- Comprehensive test suite across multiple SQL drivers (MySQL, PostgreSQL, SQL Server, SQLite, Firebird, Oracle)
- CI workflow for automated builds and test execution (`.github/workflows/ci.yml`)
- GitHub Issue templates for bug reports and feature requests
- GitHub Pull Request template
- `CHANGELOG.md` to track project changes
- `LICENSE.txt` with project licensing information
- Code coverage reporting with HTML output (`coverlet.runsettings`)
- Test helpers and custom `IQueryFactory` for integration tests

### Changed

- Updated build process to generate coverage reports
- Updated `.editorconfig` and `.gitattributes` configuration
- Updated `.gitignore` for coverage artifacts
- Updated `publish.yml` workflow
- Refactored test fixtures and test files for consistency across all drivers
- Updated project dependencies in `MVFC.SQLCraft.Tests.csproj`
- Updated `README.md` documentation

## [1.1.0] - 2026-02-20

### Added

- Centralized package support via Metapackage
- Oracle driver support (`MVFC.SQLCraft.Oracle`) with integration tests
- Built-in code analyzer (Roslyn analyzers)
- Improved test readability via FluentAssertions

### Changed

- Solution format migrated from `.sln` to `.slnx`
- Reorganized project folder structure
- Fixed file accidentally removed from git

## [1.0.2] - 2025-11-16

### Added

- .NET 10 support
- Updated CI/CD workflow

## [1.0.1] - 2025-10-30

### Added

- NuGet package icons for all projects
- Repository URL in package properties

### Fixed

- Fixes in the main README

## [1.0.0] - 2025-10-12

### Added

- First stable release of the MVFC.SQLCraft suite
- Base abstraction library (`MVFC.SQLCraft`)
- MySQL/MariaDB driver (`MVFC.SQLCraft.Mysql`)
- SQL Server driver (`MVFC.SQLCraft.MsSQL`)
- PostgreSQL driver (`MVFC.SQLCraft.PostgreSql`)
- SQLite driver (`MVFC.SQLCraft.SQLite`)
- Firebird driver (`MVFC.SQLCraft.Firebird`)
- Integration with [SqlKata](https://github.com/sqlkata/querybuilder)
- Full async API with `CancellationToken` support
- Transaction support via `ExecuteInTransactionAsync`
- Build pipeline via Cake (`build.cake`)
- `IDatabaseLogger` for query logging

[3.1.1]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.1.0...v3.1.1
[3.1.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.9...v3.1.0
[3.0.9]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.8...v3.0.9
[3.0.8]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.7...v3.0.8
[3.0.7]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.6...v3.0.7
[3.0.6]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.5...v3.0.6
[3.0.5]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.4...v3.0.5
[3.0.4]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.3...v3.0.4
[3.0.3]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.2...v3.0.3
[3.0.2]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.1...v3.0.2
[3.0.1]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.0...v3.0.1
[3.0.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.1.0...v3.0.0
[1.1.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.0.2...v1.1.0
[1.0.2]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/releases/tag/v1.0.0
