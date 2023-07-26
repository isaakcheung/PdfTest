<template>
    <!-- App.vue -->
    <v-app>
        <v-app-bar app>
            {{pageName}}
        </v-app-bar>

        <v-main>
            <v-container fluid>
                <input ref="uploader" type="file" @change="upload" /><br />
                訂票號碼:{{flyNumber}}<br />
                旅客:{{flyPassenger}}<br />
                起飛時間：{{departureTime}}<br />
                起飛機場：{{departureStation}}<br />
                降落時間：{{arrivalTime}}<br />
                降落機場：{{arrivalStation}}<br />
                文件內容：<br />
                <iframe ref="preview" style="width:100%;height:100%;">
                </iframe>
            </v-container>
        </v-main>

        <v-footer app>
        </v-footer>
    </v-app>
</template>
<script>
    module.exports = {
        data: function () {
            return {
                pageName: '機票行程擷取',
                file: null,
                htmlContent: null,
                flyNumber: "",
                flyPassenger: "",
                departureTime: "",
                departureStation: "",
                arrivalTime: "",
                arrivalStation: ""
            }
        },
        methods: {
            upload: function ()
            {
                const toBase64 = file => new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = () => resolve(reader.result);
                    reader.onerror = error => reject(error);
                });
                toBase64(this.$refs.uploader.files[0]).then((r) => {
                    var comp = this;
                    comp.file = r;
                    axios.post("/cloud/parse", { Ticket: comp.file }).then((r) => {
                        comp.htmlContent = r.data.document;
                        comp.$refs.preview.contentWindow.document.clear();
                        //comp.$refs.preview.contentWindow.addEventListener("load", (e) => {
                        //    debugger;
                        //});
                        debugger;
                        try {

                            comp.$refs.preview.contentWindow.document.write(r.data.document);
                        } catch (e) {
                            null;
                        }
                        comp.flyNumber = comp.$refs.preview.contentWindow.document.querySelector("body > div:nth-child(1) > div.stl_view > div > div:nth-child(5)").innerText.replace("MakeMyTrip Booking ID - ","");
                        comp.flyPassenger = comp.$refs.preview.contentWindow.document.querySelector("body > div:nth-child(1) > div.stl_view > div > div:nth-child(24)").innerText;
                        comp.departureTime = comp.$refs.preview.contentWindow.document.querySelector("body > div:nth-child(1) > div.stl_view > div > div:nth-child(13)").innerText;
                        comp.departureStation = comp.$refs.preview.contentWindow.document.querySelector("body > div:nth-child(1) > div.stl_view > div > div:nth-child(14)").innerText;
                        comp.arrivalTime = comp.$refs.preview.contentWindow.document.querySelector("body > div:nth-child(1) > div.stl_view > div > div:nth-child(17)").innerText;
                        comp.arrivalStation = comp.$refs.preview.contentWindow.document.querySelector("body > div:nth-child(1) > div.stl_view > div > div:nth-child(16)").innerText;
                    });
                });
            }
        }
        
    }
</script>
<style>

</style>