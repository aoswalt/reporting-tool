# Reporting Tool

The reporting tool is a C# application designed to provide easy access to lettering order details inside the art department.

Internally, the application constructs SQL statements to run on the AS400 using the Odbc connector, authenticated through Kerberos.

## Design

Report results are displayed in the lower data grid. The results can be opened in Excel or easily copied to the clipboard.

As a safety measure, results are limited to 1000 records unless the limit checkbox is cleared.

Reports can be saved and loaded from files to be reused and shared.

### Order Tab

The Order tab is focused around easy lookup of lettering related orders. The various fields can be used to modify the query.

Preset filters exist for the standard types of lettering:
* Cut Lettering
* Rhinestone / Sequins
* Sublimation
* Sew

### Query Tab

The Query tab allows for manual, read-only SQL queries to be executed in cases where the other tabs do not provide the results needed.

Queries may include question marks to prompt the user for variables. Labels may be provided to provide context to the prompts with a comma-separated list, matching the prompt inputs 1-to-1.

On the other taps, ctrl-clicking the run button will insert the constructed query into the Query tab instead of executing it, enabling quick debugging or modification.

### Custom Tab

The Custom tab provides a GUI for the user to construct custom reports.

Fields may be added, removed, and reorded as needed. Values may be given to the fields to filter the results by creating a set of where clauses ANDed together.
