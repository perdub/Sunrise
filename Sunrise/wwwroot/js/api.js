async function getUser() {
    //получает активного пользователя
    let fr = new Object();
    fr.ok = false;
    let res = await fetch("/api/users/getuser", {
        method: "GET",
        headers: {
            "Access-Control-Allow-Origin": "*",
            "mode": "no-cors",
            "Content-Type": "application/json"
        }
    });

    if (res.status === 200) {
        let json = await res.json();
        json.ok = true;
        console.log("Logged as " + json.name + ", id - " + json.id);
        return json;
    }
    if (res.status === 401) {
        return fr;
    }
}