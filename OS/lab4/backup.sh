#!/bin/bash


dir="/home/user"
source="$dir/source"
report="$dir/backup-report"

GREEN='\033[0;32m'
PURPLE='\033[0;35m'
RED='\033[0;31m'
NC='\033[0m'

# Dont ban
echo_add() {
echo -e "${GREEN}ADD${NC}: $@"
echo "ADD: $@" >> "$report"
}

echo_change() {
echo -e "${PURPLE}CNG${NC}: $@"
echo "CNG: $@" >> "$report"
}

echo_err() {
echo -e "${RED}ERR${NC}: $@"
echo -e "ERR: $@" >> "$report"
}

if [ ! -d "$dir" ]
then
        echo_err "Directory $dir doesn't exist"
        exit 1
fi

if [ ! -d "$source" ]
then
        echo_err "Directory $source doesn't exist"
        exit 1
fi

today=$(date +%F)
lastdate=$(ls "$dir" | grep -Po "Backup-\K[0-9]{4}-[0-9]{2}-[0-9]{2}" | sort | tail -n1)
if [ -n "$lastdate" ] && [ $(( ($(date --date="$today" +%s) - $(date --date="$lastdate" +%s)) / (60*60*24) )) -lt 7 ]
then
        backupdir="$dir/Backup-$lastdate"
        echo_change "Backup into $backupdir"
        for obj in $(ls "$source")
        do
                if [ ! -f "$source/$obj" ]
                then
                        continue
                fi
                if [ -f "$backupdir/$obj" ]
                then
                        src_size=$(wc -c "$source/$obj" | awk '{ print $1 }')
                        bcp_size=$(wc -c "$backupdir/$obj" | awk '{ print $1 }')
                        if [ $(( $src_size - $bcp_size )) -ne 0 ]
                        then
                                mv -- "$backupdir/$obj" "$backupdir/$obj.$today" || {
								echo_err "Can't move $obj -> $obj.$today"
								exit 1
								}
								echo_change "$obj -> $obj.$today"
                                cp "$source/$obj" "$backupdir/$obj" || {
								echo_err "Can't copy $source/$obj -> $backupdir/$obj"
								exit 1
								}
								echo_add "$obj"
                        fi
                else
                        cp -- "$source/$obj" "$backupdir/$obj" || {
						echo_err "Can't copy $source/$obj -> $backupdir/$obj"
						exit 1
						}
						echo_add "$obj"
                fi
        done
else
        backupdir="$dir/Backup-$today"
        mkdir "$backupdir" || {
		echo_err "Can't create dir $backupdir"
		exit 1
		}
		echo_add "New backup in $backupdir"
        for obj in $(ls "$source")
        do
                if [ -f "$source/$obj" ]
                then
                        cp -- "$source/$obj" "$backupdir/$obj" || {
						echo_err "Can't copy $source/$obj -> $backupdir/$obj"
						exit 1
						}
						echo_add "$obj"
                fi
        done
fi
