#!/bin/bash

get_proc_bytes() {
        grep -Pos "rchar:\K.*" $(find /proc -maxdepth 2 -mindepth 2 -regex .*io) > $1
}

get_proc_bytes step1
sleep 60
get_proc_bytes step2

echo "" > tmp_task7.log

while read i
do
if [[ -n "$i" ]]
then
        pid=$(echo "$i" | grep -Po "/proc/\K[0-9]*")
        old_data=$(grep -Po "/proc/$pid/io: \K.*" step1)
        new_data=$(echo "$i" | awk '{ print $2 }')
        if [[ -n "$old_data" ]]
        then
                let diff="$new_data"-"$old_data"
                echo "$diff $pid" >> tmp_task7.log
        fi
fi
done < step2

cat tmp_task7.log | sort -rn | head --lines=3 | awk '{ print "PID", $2, "READ", $1 }'
rm -f tmp_task7.sh
