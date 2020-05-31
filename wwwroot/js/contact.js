const emailButton = document.getElementById('email-button');
const name = document.getElementById('Name');
const email = document.getElementById('Email');
const phone = document.getElementById('Phone');
const message = document.getElementById('Message');


function isFormValid() {
    return email.value.includes('@') && name.value !== null
        && phone !== 0 && message.value !== null;
}

console.log(emailButton);
document.addEventListener('submit', function (event) {
    event.preventDefault();
    console.log(name.value);
    console.log(message.value);

    if (isFormValid()) {
        fetch("Home/SendEmail", {
            method: 'POST',
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify({ Name: name.value, Email: email.value, Phone: parseInt(phone.value), Message: message.value })
        }).then(data => data.json()).then(result => showResult(result) 
        //{
        //    console.log('wynik', result);
        //    if (result) {
        //        emailButton.disabled = true;
        //        name.value = "";
        //        name.disabled = true;
        //        phone.value = "";
        //        phone.disabled = true;
        //        email.value = "";
        //        email.disabled = true;
        //        message.value = "";
        //        message.disabled = true;
        //        emailButton.value = "Wysłano";
        //        alert("Wysłano wiadomość");
        //    }
        //    else {
        //        alert("Wiadomość nie została wysłana");
        //    }
        //}
        )
    }
    else {
        alert("Wiadomość nie została wysłana");
    }


})


function showResult(result) {
    console.log(result);
    if (result) {
        emailButton.disabled = true;
        name.value = "";
        name.disabled = true;
        phone.value = "";
        phone.disabled = true;
        email.value = "";
        email.disabled = true;
        message.value = "";
        message.disabled = true;
        emailButton.value = "Wysłano";
        alert("Wysłano wiadomość");
    }
    else {
        alert("Wiadomość nie została wysłana");
    }
}