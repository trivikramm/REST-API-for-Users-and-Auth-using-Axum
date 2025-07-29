#!/bin/bash
cd /workspaces/Login-Dashboard-Mini-Project/LoginDashboardProject/frontend
export NODE_OPTIONS=--openssl-legacy-provider
npx ng serve --host 0.0.0.0 --disable-host-check
