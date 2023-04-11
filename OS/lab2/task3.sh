#!/bin/bash

ps axo etime,pid,comm | sort | head --lines=7 | tail --lines=1 | awk '{print "Newest process PID is", $2, "COMM is", $3}'

#                | sort | head --lines=1 | egrep -o '[0-9]*$'
