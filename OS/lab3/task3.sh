#!/bin/bash
crontab -l > mycron

echo "*/5 * * * 1 task1.sh" >> mycron
crontab mycron
rm -f mycron