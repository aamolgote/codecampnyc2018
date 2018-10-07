// solium-disable linebreak-style
pragma solidity ^0.4.24;

contract Marketplace
{
    enum StateType { 
      ItemAvailable,
      OfferPlaced,
      Accepted
    }

    address private InstanceOwner;
    string private Description;
    int private AskingPrice;
    StateType private State;

    address private InstanceBuyer;
    int private OfferPrice;

    constructor(string description, int price) public
    {
        InstanceOwner = msg.sender;
        AskingPrice = price;
        Description = description;
        State = StateType.ItemAvailable;
    }

    function MakeOffer(int offerPrice) public
    {
        if (offerPrice == 0)
        {
            revert("Invalid Action");
        }

        if (State != StateType.ItemAvailable)
        {
            revert("Invalid Action");
        }
        
        if (InstanceOwner == msg.sender)
        {
            revert("Invalid Action");
        }

        InstanceBuyer = msg.sender;
        OfferPrice = offerPrice;
        State = StateType.OfferPlaced;
    }

    function Reject() public
    {
        if ( State != StateType.OfferPlaced )
        {
            revert("Invalid Action");
        }

        if (InstanceOwner != msg.sender)
        {
            revert("Invalid Action");
        }

        InstanceBuyer = 0x0;
        State = StateType.ItemAvailable;
    }

    function AcceptOffer() public
    {
        if ( msg.sender != InstanceOwner )
        {
            revert("Invalid Action");
        }

        State = StateType.Accepted;
    }

    function GetOfferPrice() public view returns(int)
    {
        return OfferPrice;
    }

    function GetAskingPrice() public view returns(int)
    {
        return AskingPrice;
    }
}