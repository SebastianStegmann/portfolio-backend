-- Add a test person to the person table
-- This person can be used for testing find_names, string_search, and other functions that require a valid personId

INSERT INTO person (name, birthday, location, email, password, created_at)
VALUES (
    'Test User',
    '1990-01-01',
    'United States',
    'testuser@example.com',
    'test123',  -- In production, this should be hashed
    NOW()
)
RETURNING id, name, email;

-- To use this person ID in your tests, note the returned ID
-- Then use it like: /api/functions/find-names?name=radcliffe&personId=<ID>&limit=10
