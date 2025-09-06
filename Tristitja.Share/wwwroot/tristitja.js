window.tristitja = {
    login,
    logout
}

async function login(username, password)
{
    if (typeof(username) != "string" || typeof(password) != "string") {
        throw new Error("Pierdolnij się w łeb");
    }
    
    var req = { username, password };
    var res = await fetch("/loginexec", {
        method: 'POST',
        body: JSON.stringify(req),
        headers: {
            "Content-Type": "application/json"
        }
    });
    
    if (res.ok)
    {
        alert("Chuj");
        location.reload(true);
        return true; // Will not return XD But whatever
    }
    
    return false;
}

async function logout()
{
    var res = await fetch("/logoutexec", { method: 'DELETE' });
    
    if (res.ok)
    {
        location.reload(true);
        return true;
    }
    
    return false;
}
