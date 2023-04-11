#!/bin/bash

while true
do
        read LINE
        if [[ -z $(echo "$LINE" | egrep -o "^(\*|\+|(\-?[1-9][0-9]*)|0)$") ]]
        then
                echo "error"
                echo "QUIT" >> pipe5
                exit 1
        fi
        echo "$LINE" >> pipe5
        if [[ "$LINE" == "QUIT" ]]
        then
                exit
        fi
done
