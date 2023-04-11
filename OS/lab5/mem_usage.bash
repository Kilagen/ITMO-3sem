#!/bin/bash

while [ true ]
do
top | head --lines=12 | tail --lines=9
sleep 1
done
