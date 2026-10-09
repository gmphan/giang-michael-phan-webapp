#!/bin/bash
BACKUP_DIR="$HOME/backups/postgres"
DATE=$(date +%Y%m%m_%H%M%S)
mkdir -p $BACKUP_DIR

# Run pg_dump inside the container
docker exec -t local_postgres pg_dumpall -U ocbuumaster > "$BACKUP_DIR/db_backup_$DATE.sql"

# Delete backups older than 7 days
find $BACKUP_DIR -type f -name "*.sql" -mtime +7 -delete
