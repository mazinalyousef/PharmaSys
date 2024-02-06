using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Barcode : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public Barcode(DataContext dataContext,IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            
        }  


        [HttpGet("{barcode}")]
        public async Task<ActionResult<BarcodeForViewDTO>> GetBarcode(string barcode)
        {
             var _result = await _dataContext.Barcodes.Where(x=>x.barcode==barcode)
             .Include(x=>x.product)
             .FirstOrDefaultAsync();
             if (_result==null)
             {
                return NotFound();
             }
             var barcodeforviewdto = _mapper.Map<BarcodeForViewDTO>(_result);
             return Ok(barcodeforviewdto);
        }
    }
}