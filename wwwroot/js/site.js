
$(function () {
    console.log('test')

    //aktualizacja ilosci zamowionych rzeczy w koszyku + zapis do bazy danych
    const rows = document.querySelectorAll('tr.position');
    const suma = document.getElementById('sum');
    const table = document.getElementById('tab');

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



    //ladowanie podgladu obrazka przy dodawaniu i edycji
  
    const imgBtn = document.getElementById('Image_ImageFile');
    const imgPreview = document.getElementById('imagePreview');
    const imgInput = document.getElementById('Image_ImageFile');
    console.log(imgBtn);
    imgBtn.addEventListener('change', function () {
        const file = this.files[0];

        if (file) {
            const reader = new FileReader();
            //var preview = document.getElementById(imgPreview);
            imgPreview.style.display = 'block';
            reader.addEventListener('load', function () {
                console.log(this);
                imgPreview.setAttribute('src', this.result);
            });
            reader.readAsDataURL(file);
        }
        else {
            //preview.style.display = '';
            imgPreview.setAttribute('src', '');
        }
        
    });



});

