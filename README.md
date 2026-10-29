# PaymentGateWayAsia

A multi-tier C# payment gateway integration project (circa 2013) — a layered
.NET rewrite of an earlier payment gateway.

## Architecture

The solution is split into classic N-tier layers:

- **DAL** — data access over ADO.NET (`System.Data.SqlClient`). `DBConnect`
  executes stored procedures with parameterised, injection-safe commands;
  `DataObject` wraps the returned reader/results.
- **BLL** — business logic layer (scaffolded).
- **Controller** — coordinates between the service layer and business logic.
- **MerchantIntegration** — a WCF service (`Service1.svc` / `IService1`)
  exposing operations for merchants to integrate with the gateway.

## Tech

C# / .NET Framework, WCF, ADO.NET, SQL Server stored procedures.

## Configuration

Database connection strings are read from configuration
(`Web.config` / `App.config`).

## Status

An older, partially-scaffolded project kept for reference.
