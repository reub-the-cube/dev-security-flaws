from flask import Flask, request, render_template
import sqlite3

app = Flask(__name__)

@app.route('/search')
def search_users():
    search_term = request.args.get('name')
    
    conn = sqlite3.connect('users.db')
    cursor = conn.cursor()
    
    # Find users matching the search
    query = f"SELECT username, email FROM users WHERE username LIKE '%{search_term}%'"
    cursor.execute(query)
    results = cursor.fetchall()
    
    return render_template('results.html', users=results)

@app.route('/admin')
def admin_panel():
    user_role = request.args.get('role')
    if user_role == 'admin':
        return render_template('admin.html')
    else:
        return "Access Denied"