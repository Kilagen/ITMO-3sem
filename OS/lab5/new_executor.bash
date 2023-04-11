#!/bin/bash

N="$1"
K="$2"
while [ "$K" -gt 0 ]
do
        ./newmem.bash "$N" &
        echo "$K"
        K=$((K-1))
        sleep 1
done
