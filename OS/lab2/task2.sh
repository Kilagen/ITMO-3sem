#!/bin/bash

# PIDs where full COMM looks like /sbin/<comm>
ps auxh | awk '$11 ~ /^\/sbin\/[^[:blank:]]/ {print $2}' > ans_task2.txt

#                   /^\/sbin\//
#                   /\/sbin\//
