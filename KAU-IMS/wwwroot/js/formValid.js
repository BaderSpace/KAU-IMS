function validateForm() {

    var fname = document.getElementById("fname").value;
    var lname = document.getElementById("lname").value;
    var email = document.getElementById("email").value;
    var pass  = document.getElementById("pass").value;

    if (fname == "" || lname == "" || email == "" || pass == "") {
        alert("Please fill in all fields.");
        return false;
    }

    if (!isNaN(fname)) {
        alert("First name must contain letters.");
        return false;
    }

    if (!isNaN(lname)) {
        alert("Last name must contain letters.");
        return false;
    }

    if (email.indexOf("@") == -1) {
        alert("Email must contain '@'.");
        return false;
    }

    if (pass.length < 6) {
        alert("Password must be at least 6 characters.");
        return false;
    }

    alert("Form submitted successfully!");
    return true;
}