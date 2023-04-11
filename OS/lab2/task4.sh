#!/bin/bash

for pid in $(ps axho pid)
do
        if [ -d "/proc/$pid" ]; then
        ppid=$(grep -Po "(?<=PPid:).*" "/proc/$pid/status")
        sum=$(grep -Po "se.sum_exec_runtime *: *\K.*" "/proc/$pid/sched")
        nr_switches=$(grep -Po "nr_switches *: *\K.*" "/proc/$pid/sched")
        art=$(echo "$sum / $nr_switches" | bc -l)

        echo "$ppid $pid $art"
        fi
done | sort -n |
awk '{printf "ProcessID=%s : Parent_ProcessID=%s : Average_Running_Time=%f\n", $2, $1, $3}' > ans_task4.txt
