#!/bin/bash

echo "" > report.log
array=()
i=0
while [ true ]
do
        array+=(1 2 3 4 5 6 7 8 9 10)
        i=$((i+1))
        if [[ $((i % 100000)) -eq 0 ]]
        then
                echo "${#array[@]}" >> report.log
        fi
done
