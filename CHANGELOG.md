# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


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

[3.0.3]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.2...v3.0.3
[3.0.2]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.1...v3.0.2
[3.0.1]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v3.0.0...v3.0.1
[3.0.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.1.0...v3.0.0
[1.1.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.0.2...v1.1.0
[1.0.2]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/releases/tag/v1.0.0
