#!/bin/bash

print_avg() {
        avg=$(echo "$2 / $3" | bc -l)
        echo "Average_Running_Children_of_ParentID=$1 is $avg" >> ans_task5.txt
}

last=0
sum_art=0
count=0
echo "" > ans_task5.txt
while read i
do
        if [[ -n "$i" ]]
        then
                cur_art=$(echo "$i" | grep -Po "Average_Running_Time=\K[0-9]*\.[0-9]{6}")
                ppid=$(echo "$i" | grep -Po "Parent_ProcessID=\K[0-9]*")

                if [[ "$last" == "$ppid" ]]
                then
                        sum_art=$(echo "$sum_art + $cur_art" | bc -l)
                        let count=count+1
                else
                        print_avg $last $sum_art $count
                        sum_art="$cur_art"
                        let count=1
                        last="$ppid"
                fi
                echo "$i" >> ans_task5.txt
        fi
done < ans_task4.txt

print_avg $last $sum_art $count
