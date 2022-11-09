﻿using Microsoft.AspNetCore.Mvc;
using StoreService.Middleware;
using StoreService.Service;
using System.ComponentModel.DataAnnotations;
using TakeFood.VoucherService.Model.Entities;
using TakeFood.VoucherService.Service;
using TakeFood.VoucherService.ViewModel.Dtos.Voucher;

namespace TakeFood.VoucherService.Controllers;

public class VoucherController : BaseController
{
    public IVoucherService VoucherService { get; set; }
    public IJwtService JwtService { get; set; }
    public VoucherController(IVoucherService VoucherService, IJwtService jwtService)
    {
        this.VoucherService = VoucherService;
        this.JwtService = jwtService;
    }

    [HttpPost]
    [Authorize(roles: Roles.ShopeOwner)]
    [Route("AddVoucher")]
    public async Task<IActionResult> AddVoucherAsync([FromBody] CreateVoucherDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await VoucherService.CreateVoucherAsync(dto, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize(roles: Roles.ShopeOwner)]
    [Route("UpdateVoucher")]
    public async Task<IActionResult> UpdateVoucherAsync([FromBody] UpdateVoucherDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await VoucherService.UpdateVoucherAsync(dto, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("GetVoucher")]
    public async Task<IActionResult> GetVoucherAsync([Required] string storeId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await VoucherService.GetAllStoreVoucherOkeAsync(storeId);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize(roles: Roles.ShopeOwner)]
    [Route("GetPagingVoucher")]
    public async Task<IActionResult> GetPagingVoucherAsync(GetPagingVoucherDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await VoucherService.GetPagingVoucher(dto, GetId());
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Authorize(roles: Roles.ShopeOwner)]
    [Route("DeleteVoucher")]
    public async Task<IActionResult> GetPagingVoucherAsync([Required] string voucherId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await VoucherService.DeleteVoucherAsync(voucherId, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    public string GetId()
    {
        String token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
        return JwtService.GetId(token);
    }
    public string GetId(string token)
    {
        return JwtService.GetId(token);
    }
}
