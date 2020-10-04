
from watson_developer_cloud import VisualRecognitionV3 as vr
import os
import json

API_KEY = os.environ.get('API_PASSWORD')



instance = vr(api_key='paste your api _key here', version='2020-10-03')
pathRawImages = 'E:\TCC\YUVA EB DATASET-20200928T152820Z-001\YUVA EB DATASET\RAW IMAGES'

for dirname in os.listdir('.'):
    for filename in os.listdir(dirname):
        pathIBM = 'E:\\TCC\\source\\OCRComparer\\results\\IBM\\'
        nomeArquivoImagem = pathIBM + dirname + '\\' + filename
        if(filename.split('.')[1] == 'JPG'):
            nomeArquivoJson = nomeArquivoImagem.split('.')[0] + '.json'
            img = instance.recognize_text(images_file='url-path-to-img.jpg')
            with open(nomeArquivoJson, 'w') as outfile:
                json.dump(img, outfile)




# you can run this code in the interpreter. If you request >>> img it will output a json formatted result
# going down the json tree, you can retrieve the text in the image with the following command:

#print(img['images'][0]['text'])