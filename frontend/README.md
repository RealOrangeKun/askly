# Askly Frontend

A modern, responsive chat-based frontend for the Askly Q&A platform built with vanilla HTML, CSS, JavaScript, and Tailwind CSS.

> 🚀 **Live Repository**: [https://github.com/RealOrangeKun/askly](https://github.com/RealOrangeKun/askly)

## 📁 Project Structure

```
frontend/
├── index.html              # Root redirect page
├── README.md              # This file
├── .gitignore            # Git ignore rules
├── html/                 # All HTML pages
│   ├── index.html        # Main homepage
│   ├── chat.html         # Chat interface
│   ├── admin.html        # Admin panel
│   ├── help.html         # Help Center
│   ├── contact.html      # Contact page
│   ├── privacy.html      # Privacy Policy
│   └── terms.html        # Terms of Service
├── css/                  # Stylesheets
│   └── styles.css        # Main styles
└── js/                   # JavaScript files
    ├── chat.js           # Chat functionality
    ├── admin.js          # Admin panel logic
    └── tailwind.config.js # Tailwind configuration
```

## Features

### 🏠 **Landing Page**
- Beautiful app icon and branding
- Feature showcase with animated cards
- Call-to-action to start chatting
- Conditional admin panel access (when API key is set)
- Modern gradient design

### 💬 **Chat Interface**
- WhatsApp/Telegram-style chat experience
- Real-time conversation flow
- Two modes: Search and Create
- Auto-scrolling messages
- Responsive design for all devices
- Admin panel link in header (when API key is set)

### � **Search Mode**
- Find similar answered questions using vector similarity
- Results displayed as formatted chat messages
- No authentication required

### ➕ **Create Mode**
- Submit new questions to the platform
- Automatic embedding generation on the backend
- Success feedback in chat format

### 🛡️ **Admin Panel**
- Question management interface
- Answer pending questions
- Edit existing answers
- Pagination and filtering
- Statistics dashboard
- API key authentication

## Getting Started

### 1. Open the Application
Simply open `index.html` in your web browser to see the landing page, then click "Start Chatting" to access the chat interface.

### 2. Chat Interface Usage
- **Default Mode**: Search mode is active by default
- **Switch Modes**: Click the mode buttons at the bottom
- **Send Messages**: Type your question and press Enter or click the send button
- **View Results**: Similar questions and answers appear as chat messages

### 3. Admin Access Setup
To access the admin panel:

1. **Set API Key**: You need to manually set the API key in localStorage:
   ```javascript
   // Open browser console (F12) and run:
   localStorage.setItem('askly_api_key', 'your-admin-api-key-here');
   ```

2. **Access Admin Panel**: Once the API key is set:
   - The "Admin Panel" button will appear on the landing page
   - An "Admin" link will appear in the chat header
   - Click either to access the admin interface

3. **Admin Features**:
   - View all questions with pagination
   - Filter by answered/unanswered status
   - Answer pending questions
   - Edit existing answers
   - Real-time statistics

## File Structure

```
frontend/
├── index.html          # Landing page with app showcase
├── chat.html          # Chat interface  
├── chat.js            # Chat functionality and API integration
├── admin.html         # Admin panel interface
├── admin.js           # Admin functionality and API integration
├── app.js             # Legacy file (can be removed)
├── styles.css         # Custom CSS styles
└── README.md          # This documentation
```

## Design Features

### 🎨 **Modern UI/UX**
- **Landing Page**: Clean hero section with app icon, features, and CTA
- **Chat Interface**: Message bubbles, typing indicators, and smooth animations
- **Responsive Design**: Works perfectly on desktop, tablet, and mobile
- **Brand Colors**: Purple/blue gradient theme throughout

### � **Chat Experience**
- **User Messages**: Right-aligned with gradient background
- **Bot Messages**: Left-aligned with assistant branding
- **Auto-resize Input**: Textarea grows/shrinks based on content
- **Mode Switching**: Visual indicators for Search vs Create modes
- **Loading States**: Elegant loading spinner during API calls

### 📱 **Responsive Features**
- Mobile-optimized chat interface
- Touch-friendly buttons and inputs
- Proper keyboard handling on mobile devices

## API Integration

The chat interface integrates with your Askly API endpoints:

- `GET /api/questions/ask` - Search similar questions (Search Mode)
- `POST /api/questions` - Create new questions (Create Mode)

## Browser Compatibility

Works in all modern browsers:
- Chrome 60+
- Firefox 55+
- Safari 12+
- Edge 79+

## Configuration

### API Base URL
The API base URL is configured in `chat.js`:
```javascript
const API_BASE_URL = 'http://localhost:5000/api';
```

Change this if your backend runs on a different port or domain.

## User Journey

1. **Landing Page** (`index.html`): Users see the app showcase and click "Start Chatting"
2. **Chat Interface** (`chat.html`): Users interact with the AI assistant
3. **Search Mode**: Ask questions to find similar Q&As
4. **Create Mode**: Submit new questions to the knowledge base
5. **Navigation**: Users can return to the home page anytime

## Troubleshooting

### CORS Issues
If you encounter CORS errors, ensure your backend API includes the appropriate CORS headers for your frontend domain.

### API Connection Issues
1. Verify your backend is running on `http://localhost:5000`
2. Check the browser's developer console for error messages
3. Ensure the API endpoints match the OpenAPI specification

## Future Enhancements

Potential improvements you could add:
- Message search and history
- File upload capabilities
- Voice input support
- Real-time typing indicators
- Message reactions and voting
- User avatars and profiles
- Conversation persistence

## 🚀 Deployment

### GitHub Pages
This project is ready for GitHub Pages deployment:

1. **Enable GitHub Pages** in your repository settings
2. **Select Source**: Deploy from `main` branch
3. **Access**: Your site will be available at `https://yourusername.github.io/askly/`

### Local Development
```bash
# Clone the repository
git clone https://github.com/RealOrangeKun/askly.git
cd askly/frontend

# Open in browser
# Simply open index.html in your web browser
# Or use a local server:
python -m http.server 8000
# Navigate to http://localhost:8000
```

### Custom Domain
To use a custom domain:
1. Add a `CNAME` file to the root with your domain
2. Configure DNS to point to GitHub Pages
3. Enable HTTPS in repository settings

## 🔗 API Integration

Make sure your backend API is running and accessible. Update the API endpoints in:
- `js/chat.js` - For chat functionality
- `js/admin.js` - For admin operations

## 📞 Support

- **Repository**: [https://github.com/RealOrangeKun/askly](https://github.com/RealOrangeKun/askly)
- **Issues**: Report bugs and feature requests on GitHub
- **Documentation**: See the Help Center in the application

## 📄 License

This project is open source. See the LICENSE file for details.

---

Enjoy your modern chat-based Askly experience! 🚀
