import { environment } from './environments/environment';

// Check if we can connect to the API
const apiUrl = environment.apiUrl;
console.log('API URL:', apiUrl);

// Fetch the root endpoint
fetch(apiUrl.replace('/api', ''))
  .then(response => {
    console.log('Root endpoint status:', response.status);
    return response.text();
  })
  .then(text => {
    console.log('Root endpoint response:', text);
  })
  .catch(error => {
    console.error('Error connecting to root endpoint:', error);
  });

// Try fetching the login endpoint
fetch(`${apiUrl}/auth/login`, {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    username: 'testuser',
    password: 'password'
  })
})
  .then(response => {
    console.log('Login endpoint status:', response.status);
    return response.json();
  })
  .then(data => {
    console.log('Login response:', data);
  })
  .catch(error => {
    console.error('Error connecting to login endpoint:', error);
  });
