#!/bin/bash

dir="/home/user"
restoredir="$dir/restore"

if [ ! -d "$dir" ]
then
        echo "No directory $dir for backups"
        exit 1
fi

lastbackup=$(ls "$dir" | egrep "^Backup-[0-9]{4}-[0-9]{2}-[0-9]{2}$" | sort | tail -n1)

if [ -z "$lastbackup" ]
then
        echo "No backups to restore"
        exit 1
fi

if [ ! -d "$restoredir" ]
then
        mkdir "$restoredir" || {
		echo "Can't create dir $restoredir" 1>&2
		exit 1
		}
fi

backupdir="$dir/$lastbackup"

nameconflict=0
for obj in $(ls "$backupdir" | grep -Ev "\.[0-9]{4}-[0-9]{2}-[0-9]{2}$")
do
        if [ ! -f "$backupdir/$obj" ]
        then
                continue
        fi
        if [ -d "$restoredir/$obj" ]
        then
                echo "Name conflict with dir $restoredir"
                nameconflict=1
        fi
done

if [ "$nameconflict" -ne 0 ]
then
        echo "Restoring failed due to name conflicts"
        exit 1
fi

for obj in $(ls "$backupdir" | grep -Ev "\.[0-9]{4}-[0-9]{2}-[0-9]{2}$")
do
        if [ ! -f "$backupdir/$obj" ]
        then
                continue
        fi
        cp -- "$backupdir/$obj" "$restoredir/$obj" || {
		echo "Can't copy $backupdir/$obj -> $restoredir/$obj"
		exit 1
		}
done
