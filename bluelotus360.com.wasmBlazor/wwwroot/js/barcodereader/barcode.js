(function () {
    window.zxing = {
        start: function (autostop, wrapper) {


            let selectedDeviceId;
            //const codeReader = new ZXing.BrowserBarcodeReader()
            const codeReader = new ZXing.BrowserMultiFormatReader()

            codeReader.getVideoInputDevices()
                .then((videoInputDevices) => {
                    const sourceSelect = document.getElementById('sourceSelect')
                    selectedDeviceId = videoInputDevices[0].deviceId

                    if (videoInputDevices.length > 1) {
                        videoInputDevices.forEach((element) => {
                            const sourceOption = document.createElement('option')
                            sourceOption.text = element.label
                            sourceOption.value = element.deviceId
                            sourceSelect.appendChild(sourceOption)
                            selectedDeviceId = element.deviceId;
                        })

                        sourceSelect.onchange = () => {
                            selectedDeviceId = sourceSelect.value;
                            codeReader.reset();
                            StartScan();
                        }

                        const sourceSelectPanel = document.getElementById('sourceSelectPanel')
                        sourceSelectPanel.style.display = 'block'
                    }


                    //StartScan(autostop);

                    document.getElementById('startButton').addEventListener('click', () => {
                        var img = document.getElementById('qr-img');
                        var v = document.getElementById('video');

                        if (!img.classList.contains('d-none') && v.classList.contains('d-none')) {
                            img.classList.add('d-none');
                            v.classList.remove('d-none');

                        }




                        selectedDeviceId = sourceSelect.value;
                        codeReader.reset();
                        StartScan();




                    })

                    document.getElementById('resetButton').addEventListener('click', () => {
                        var img = document.getElementById('qr-img');
                        var v = document.getElementById('video');

                        document.getElementById('result').textContent = '';
                        selectedDeviceId = null;
                        codeReader.reset();
                        if (img.classList.contains('d-none')) {
                            img.classList.remove('d-none');

                        }

                        if (!v.classList.contains('d-none')) {

                            v.classList.add('d-none');
                        }


                    })

                    //document.getElementById('rotateButton').addEventListener('click', () => {

                    //    var v = document.getElementById('video');

                    //    navigator.mediaDevices.getUserMedia({

                    //        video: {

                    //            facingMode: {
                    //                exact: 'environment'
                    //            }

                    //        }
                    //    }).then(stream => {
                    //        window.localStream = stream;
                    //        v.srcObject = stream;

                    //        console.log("camera rotated");

                    //    }).catch((err) => {
                    //        console.log(err);
                    //    });;


                    //})

                    //document.getElementById('closeButton').addEventListener('click', () => {
                    //    document.getElementById('result').textContent = '';
                    //    codeReader.reset();
                    //    console.log('closeButton.')
                    //    wrapper.invokeMethodAsync("invokeFromJSClose");
                    //})

                    document.getElementById('cancelButton').addEventListener('click', () => {
                        var img = document.getElementById('qr-img');
                        var v = document.getElementById('video');

                        document.getElementById('result').textContent = '';
                        selectedDeviceId = null;
                        codeReader.reset();

                        if (img.classList.contains('d-none')) {
                            img.classList.remove('d-none');

                        }

                        if (!v.classList.contains('d-none')) {

                            v.classList.add('d-none');
                        }

                        window.history.back();

                    })

                    function StartScan() {
                        codeReader.decodeOnceFromVideoDevice(selectedDeviceId, 'video').then((result) => {
                            var img = document.getElementById('qr-img');
                            var v = document.getElementById('video');

                            console.log(result)
                            document.getElementById('result').textContent = result.text

                            var supportsVibrate = "vibrate" in navigator;
                            if (supportsVibrate) navigator.vibrate(1000);

                            if (autostop) {
                                console.log('autostop');
                                codeReader.reset();
                                return wrapper.invokeMethodAsync("invokeFromJS", result.text);
                            } else {
                                console.log('None-stop');
                                codeReader.reset();
                                wrapper.invokeMethodAsync("invokeFromJS", result.text);
                            }

                            if (img.classList.contains('d-none')) {
                                img.classList.remove('d-none');

                            }

                            if (!v.classList.contains('d-none')) {

                                v.classList.add('d-none');
                            }

                        }).catch((err) => {
                            console.error(err)
                            document.getElementById('result').textContent = err
                        })
                        console.log(`Started continous decode from camera with id ${selectedDeviceId}`)
                    }





                })
                .catch((err) => {
                    console.error(err)
                })
        }


    };

})();