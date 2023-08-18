//функция для создания обьектов в dom которые показывают прогресс загрузки
function createUpload()
{
    var d = document.createElement('div');
    d.classList.add('item-upload-progress');

    var prbar = document.createElement('progress');
    prbar.classList.add('progress-bar-style');
    prbar.max = 100;
    prbar.value = 0;

    d.appendChild(prbar);
    document.getElementById('upload-progress').appendChild(d);

    return prbar;
}

//загружает файл
async function uploadFile(file, activeItems, domprogress) {

    var n = file.name.split('.');
    var ext = n[n.length-1];

    var config = {
        onUploadProgress: function(p){
            domprogress.value = Math.round((p.loaded * 100) / p.total);
        },
        validateStatus: function (status) {
            //пометка всех статусов валидными для того чтобы не ловить исключения
            return true;
        }
    };

    var file = await axios.post(
        "/upload/item?extension="+ext, 
        file,
        config
    );
    activeItems.counter--;
}
//обрабатывает массив файлов или же filelist
async function processFiles(files) {

    var pb = new Queue();

    //создание очереди и добавление в неё обьектов
    let queue = new Queue();
    for (let i = 0; i < files.length; i++) {
        queue.enqueue(files[i]);
        pb.enqueue(createUpload());
    }
    //создание счётчика
    const maxActiveUploads = 5;
    var activeItems = {
        counter: 0
    }
    //отправка очереди
    while (!queue.isEmpty) {
        if (activeItems.counter < maxActiveUploads) {
            activeItems.counter++;
            uploadFile(
                queue.dequeue(),
                activeItems,
                pb.dequeue()
            );
        }
        else {
            await sleep(250);
        }
    }
}
//функция инициализации
function initUpload() {
    //обьявление переменных
    var uploadtarget = document.getElementById('target');
    const p = 191;
    let inp = false, out = false;

    //подписска на события входа выхода мыши
    uploadtarget.onmouseenter = enter;
    uploadtarget.onmouseleave = leave;
    //обработка клика
    var fileInput = document.getElementById('fileInput');
    fileInput.onchange = () => {
        var files = fileInput.files;
        processFiles(files);
    };
    uploadtarget.onclick = () => {
        fileInput.click();
    };
    //подписка на события драг-дропа
    uploadtarget.ondragover = dragOverHandler;
    uploadtarget.ondrop = dropHandler;
    uploadtarget.ondragleave = leave;

    //эта функция преопределяет дефолтное поведение и вызывает функцию выделения
    function dragOverHandler(a) {
        a.preventDefault();
        enter();
    }
    //получает файлы и добавляет их в массив, после чего отправляет
    function dropHandler(a) {
        var files = [];
        a.preventDefault();

        if (a.dataTransfer.items) {
            // Use DataTransferItemList interface to access the file(s)
            [...a.dataTransfer.items].forEach((item, i) => {
                // If dropped items aren't files, reject them
                if (item.kind === "file") {
                    files.push(item.getAsFile());
                }
            });
        } else {
            // Use DataTransfer interface to access the file(s)
            [...a.dataTransfer.files].forEach((file, i) => {
                files.push(file);
            });
        }
        processFiles(files);
    }
    //функция которая перекрашивает div для загрузки когда курсор над ним
    async function enter(){
        inp = true;
        out = false;
        for (let step = 0.0; step < 1.0; step += 0.02) {
            if (inp === true) {
                var alphaChannel = Math.round(40 + (step * p));
                uploadtarget.style.background = "#4f6f8e" + alphaChannel.toString(16);
                await sleep(10);
            }
            else {
                break;
            }
        }
        inp = false;
    }
    //функция которая возвращает его в начальное состояние
    async function leave(){
        inp = false;
        out = true;
        for (let step = 1.0; step > 0.0; step -= 0.02) {
            if (out === true) {
                var alphaChannel = Math.round(40 + (step * p));
                uploadtarget.style.background = "#4f6f8e" + alphaChannel.toString(16);
                await sleep(10);
            }
            else {
                break;
            }
        }
        out = false;
    }
}