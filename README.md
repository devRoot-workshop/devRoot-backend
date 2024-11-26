# Quick Start

To build and run the application:

```sh
make build run
```

or on windows:

```bat
.\setup.ps1 build run
```

This command will compile the backend application and execute it.

And to clean up all leftover binaries:

```sh
make clean
```

and on windows run:

```bat
.\setup.ps1 clean
```

To update the database run:

```sh
make migrate
```

On windows:

```bat
.\setup.ps1 migrate
```

*Note: You need to run the development container, or use a working postgresql database for this to work.*

## Available Targets

- `build`: Builds the backend application
- `run`: Runs the built backend application
- `clean`: Cleans up all leftover binaries
- `migrate`: Creates a migration, and updates the database with it

<hr>

# Running the development database

To run the database simply start the `postgres:15` container defined in the `docker-compose.yml` file, by running:

```sh
docker compose up -d
```


*Note: This container should only be used for development purposes.*