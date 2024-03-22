import { Component, OnInit } from '@angular/core';
import JsBarcode  from 'jsbarcode';
import { sticker } from '../_models/sticker';
import { StickersService } from '../_services/stickers.service';

@Component({
  selector: 'app-barcode-stickers',
  templateUrl: './barcode-stickers.component.html',
  styleUrls: ['./barcode-stickers.component.css']
})
export class BarcodeStickersComponent implements OnInit

{

  sticker:sticker;
  mazin:string;
  repeatCount:number;


  constructor(private stickerservice:StickersService)
  {

  }

  ngOnInit(): void
   {
    this.sticker=this.stickerservice.sticker;
    
    this.sticker.ingredients.forEach((element=>
      {

      //  console.log(element.ingredientName);

      let  stickercontainerDiv = document.createElement('div');
      stickercontainerDiv.id=element.ingredientName;
      stickercontainerDiv.className="stickerDiv";
      stickercontainerDiv.style.border="dashed";
      stickercontainerDiv.style.margin="2px"
      stickercontainerDiv.style.borderWidth="1px";
      stickercontainerDiv.style.width="250px";
      stickercontainerDiv.style.padding="5px";
      document.getElementById("print-section").appendChild(stickercontainerDiv); 
    


        
        var p_batchNo = document.createElement("p"); //create the paragraph tag
        document.getElementById(element.ingredientName).appendChild(p_batchNo); 
        p_batchNo.innerHTML = "BatchNO:"+this.sticker.batchNo;  


        var p_productName = document.createElement("p"); //create the paragraph tag
        document.getElementById(element.ingredientName).appendChild(p_productName); 
        p_productName.innerHTML = this.sticker.productName; 


        var p_ingredientName = document.createElement("p"); //create the paragraph tag
        document.getElementById(element.ingredientName).appendChild(p_ingredientName); 
        p_ingredientName.innerHTML = element.ingredientName; 

        var pqtyPerBatch = document.createElement("p"); //create the paragraph tag
        document.getElementById(element.ingredientName).appendChild(pqtyPerBatch); 
        pqtyPerBatch.innerHTML ="QTYPerBatch:"+ element.qtyPerBatch.toString(); 

        var pqtyPerTube = document.createElement("p"); //create the paragraph tag
        document.getElementById(element.ingredientName).appendChild(pqtyPerTube); 
        pqtyPerTube.innerHTML = "QTYPerTube:"+ element.qtyPerTube.toString(); 






          
        let node = document.createElementNS("http://www.w3.org/2000/svg", "svg");
        node.setAttribute("id", "barcode" + element.ingredientName);
        document.getElementById(element.ingredientName).appendChild(node); 
        JsBarcode("#barcode"+  element.ingredientName,this.sticker.barcode, {
          format: 'code128', 
          text: '-' +  this.sticker.barcode + '-',
          background: 'rgba(255,255,255,1)',
          font: 'monospace',
          fontOptions: 'bold',
          fontSize: 16,
          lineColor: 'rgba(0,0,0,1)',
          width:1.5,
          height:42
        }); 
      }))
   


   // JsBarcode(".barcode").init();
     


/*
    this.sticker.ingredients.forEach((element=>
      {
       
      }))
*/
  
  
    /*
    this.mazin="mazin is great >>>>";
    JsBarcode('#barcode',  this.mazin, {
      format: 'code128', 
      text: '-' +  this.mazin + '-',
      background: 'rgba(0,0,0,0.1)',
      font: 'monospace',
      fontOptions: 'bold',
      fontSize: 16,
      lineColor: 'darkblue',
      width:1
      
    });
    */

  }

}
