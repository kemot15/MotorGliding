$(function () {
    console.log('test')

    //aktualizacja ilosci zamowionych rzeczy w koszyku + zapis do bazy danych
    const rows = document.querySelectorAll('tr.position');
    const suma = document.getElementById('sum');
    const table = document.getElementById('tab');
    console.log(table);

    for (const tr of rows) {
        let price = tr.querySelector('.price > input');
        let partSum = tr.querySelector('.part_sum');
        let qty = tr.querySelector('.qty > input');
        let id = tr.querySelector('#detail_Id');
        qty.addEventListener('change', function () {
            var val = qty.value * price.value;
            let tempSum = suma.innerText - partSum.innerText;
            partSum.innerText = val;
            suma.innerText = tempSum + val;
            console.log(val);
            console.log(suma.innerText);
            if (table == null) {
                fetch("/Order/RefreshPosition", {
                    method: 'POST',
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify([{ id: parseInt(id.value), quantity: parseInt(qty.value) }])
                })//.then(data => data.json()).then(result => console.log(result))
            }
            

            
        })
    }

    //ladowanie podgladu obrazka przy dodawaniu i edycji
    function ShowImagePreview(imageUploader, previewImage) {
        if (imageUploader.files && imageUploader.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(previewImage).attr('src', e.target.result);
            }
            reader.readAsDataURL(imageUploader.files[0]);
        }
    }
    //function refresh() {
            //    setInterval(function () {
            //        console.log('odliczam');
            //        fetch("/Weather/Index", {
            //            method: 'GET',
            //            // headers: { 'Content-Type': "application/json" },
            //            // body: JSON.stringify([{ id: parseInt(id.value), quantity: parseInt(qty.value) }])
            //        })
            //    }, 2000);
            //}
            //refresh().then(console.log('odliczam2'));

            //.then(data => data.json()).then(result => console.log(result))


    //fetch('https://blockchain.info/ticker')
    //    .then(response => response.json()
    //        .then(value => {
    //            for (val in value) {
    //                console.log(value[val]);
    //            }
    //        })
    //    )

    //async function getCurrency() {
    //    const currencies = await fetch('https://blockchain.info/ticker');
    //    const json = await currencies.json();
    //    console.log(json);
    //}

    //getCurrency();
});