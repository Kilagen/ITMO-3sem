#!/bin/bash

max_mem=0
max_mem_pid=""
for pid in $(ps axho pid)
do
        mem=$(grep -Pos "VmRSS: *\K.*(?= kB)" "/proc/$pid/status")
        if [[ ( -n "$mem" ) && ( "$mem" -gt "$max_mem" ) ]]
        then
        let max_mem="$mem"
        max_mem_pid="$pid"
        fi
done
echo "PID: $max_mem_pid Max Memory Usage: $max_mem kB"