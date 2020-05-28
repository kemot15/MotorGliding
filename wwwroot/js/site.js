$(function () {
console.log('test')
    const rows = document.querySelectorAll('tr.position');
    const suma = document.getElementById('sum');

    for (const tr of rows) {
        let price = tr.querySelector('.price > input');
        let partSum = tr.querySelector('.part_sum');
        let qty = tr.querySelector('.qty > input');
        let id = tr.querySelector('#detail_Id');
        //console.log(eventId.value);
        //console.log(qty.value);
        //console.log(price);
        qty.addEventListener('change', function () {
            var val = qty.value * price.value;
            let tempSum = suma.innerText - partSum.innerText;
            partSum.innerText = val;
            suma.innerText = tempSum +val;
            console.log(val);
            console.log(suma.innerText);


            console.log(id.value);
            console.log(qty.value);
            console.log(price);

            fetch("/Order/RefreshPosition", {
                method: 'POST',
                headers: { 'Content-Type': "application/json" },
                body: JSON.stringify([{ id: parseInt(id.value), quantity: parseInt(qty.value) }])
            }).then(data => data.json()).then(result => console.log(result))

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
        })
    }



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