#!/bin/bash

./task4inf_cycle.sh &
pid1="$!"

./task4inf_cycle.sh &
pid2="$!"

./task4inf_cycle.sh &
pid3="$!"

# holds cpu somewhere from 5 to 10 percent
# but ofc works with delay
while true
do
        # gets cpu usage AB.C and convertes to ABC via sed
        cpu=$(top -p $pid1 -b -n 2 -d 0.4 | tail -1 | awk '{ printf "%.0f", $9 }')
        ni=$(ps -q $pid1 -o ni=)
        echo "CPU usage $cpu"
        if [[ "$cpu" -gt 10 ]]
        then
                ni=$((ni+1))
                renice "$ni" -p "$pid1"
        fi
        if [[ 5 -gt "$cpu" ]]
        then
                ni=$((ni-1))
                renice "$ni" -p "$pid1"
        fi
done &

pid4="$!"

sleep 10

kill $pid3 && echo "Process 3 was killed"

sleep 10

kill $pid1 $pid2 $pid4
