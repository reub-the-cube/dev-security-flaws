const express = require('express');
const app = express();

app.get('/api/profile/:userId', (req, res) => {
    const userId = req.params.userId;
    const user = database.getUserById(userId);
    
    if (user) {
        res.json({
            id: user.id, username: user.username,
            email: user.email, ssn: user.ssn,
            salary: user.salary,
            password_hash: user.password_hash
        });
    } else {
        res.status(404).json({ error: 'User not found' });
    }
});

app.post('/api/upload', (req, res) => {
    const filename = req.body.filename;
    const content = req.body.content;
    fs.writeFileSync(`./uploads/${filename}`, content);
    res.json({ message: 'File uploaded successfully' });
});

app.get('/api/ping', (req, res) => {
    const host = req.query.host;
    const cmd = `ping -c 4 ${host}`;
    exec(cmd, (error, stdout, stderr) => {
        res.send(stdout);
    });
});