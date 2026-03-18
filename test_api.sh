#!/bin/bash

# --- Configuration (Production Port 8080) ---
BASE_URL="http://localhost:8080/api"
API_KEY="MySecureApiKey123"
SEED_ADMIN="admin@company.com"
SEED_PW="Admin123!"

# Unique test users
USER_EMAIL="standard_$(date +%s)@example.com"
USER_PW="UserPass123!"

echo "🚀 Starting Production RBAC Security Suite..."
echo "--------------------------------------------"

# --- SCENARIO 1: THE SYSTEM ADMIN (BOOTSTRAP) ---
echo "Step 1: Logging in as System Admin..."
ADMIN_JSON=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "X-API-KEY: $API_KEY" \
  -H "Content-Type: application/json" \
  -d "{\"email\": \"$SEED_ADMIN\", \"password\": \"$SEED_PW\"}")

ADMIN_TOKEN=$(echo $ADMIN_JSON | jq -r '.token')

if [ "$ADMIN_TOKEN" == "null" ]; then
    echo "❌ ERROR: Admin login failed. Ensure Docker is running and DB is seeded."
    exit 1
fi
echo "✅ Admin Authenticated."

# --- SCENARIO 2: ADMIN REGISTERS A STANDARD USER ---
echo "Step 2: Admin creating a 'User' account ($USER_EMAIL)..."
REG_RESP=$(curl -s -X POST "$BASE_URL/auth/register" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "X-API-KEY: $API_KEY" \
  -H "Content-Type: application/json" \
  -d "{\"email\": \"$USER_EMAIL\", \"password\": \"$USER_PW\", \"role\": \"User\"}")

echo "Response: $REG_RESP"

# --- SCENARIO 3: STANDARD USER PERMISSIONS ---
echo "Step 3: Logging in as the new Standard User..."
USER_JSON=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "X-API-KEY: $API_KEY" \
  -H "Content-Type: application/json" \
  -d "{\"email\": \"$USER_EMAIL\", \"password\": \"$USER_PW\"}")

USER_TOKEN=$(echo $USER_JSON | jq -r '.token')
echo "✅ User Authenticated."

echo "Testing User Access..."
# Should be 200
CODE_W=$(curl -s -o /dev/null -w "%{http_code}" -X GET "$BASE_URL/weatherforecast" \
  -H "X-API-KEY: $API_KEY" \
  -H "Authorization: Bearer $USER_TOKEN")

# Should be 403
CODE_A=$(curl -s -o /dev/null -w "%{http_code}" -X GET "$BASE_URL/weatherforecast/admin-stats" \
  -H "X-API-KEY: $API_KEY" \
  -H "Authorization: Bearer $USER_TOKEN")

echo "-> Weather Access: $CODE_W (Expected 200)"
echo "-> Admin Stats Access: $CODE_A (Expected 403)"

# --- SCENARIO 4: ADMIN ACCESS VERIFICATION ---
echo "Step 4: Testing Admin Access to Stats..."
CODE_AA=$(curl -s -o /dev/null -w "%{http_code}" -X GET "$BASE_URL/weatherforecast/admin-stats" \
  -H "X-API-KEY: $API_KEY" \
  -H "Authorization: Bearer $ADMIN_TOKEN")

echo "-> Admin Stats Access: $CODE_AA (Expected 200)"

# --- FINAL VALIDATION ---
echo "--------------------------------------------"
if [ "$CODE_W" == "200" ] && [ "$CODE_A" == "403" ] && [ "$CODE_AA" == "200" ]; then
    echo "🎉 ALL SCENARIOS PASSED: RBAC is Secure!"
else
    echo "❌ SECURITY VALIDATION FAILED."
    echo "Check if Program.cs has: RoleClaimType = ClaimTypes.Role"
fi