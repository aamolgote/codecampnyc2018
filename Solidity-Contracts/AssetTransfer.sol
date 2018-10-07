// solium-disable linebreak-style
pragma solidity ^0.4.24;
contract AssetTransfer
{
    enum StateType { Active, OfferPlaced, PendingInspection, Inspected, Appraised, NotionalAcceptance, BuyerAccepted, SellerAccepted, Accepted, Terminated }
    address private InstanceOwner;
    string private Description;
    uint private AskingPrice;
    StateType private State;

    address private InstanceBuyer;
    uint private OfferPrice;
    address private InstanceInspector;
    address private InstanceAppraiser;

    constructor(string description, uint256 price) public
    {
        InstanceOwner = msg.sender;
        AskingPrice = price;
        Description = description;
        State = StateType.Active;
    }

    function Terminate() public
    {
        if (InstanceOwner != msg.sender)
        {
            revert("Invalid Action");
        }

        State = StateType.Terminated;
    }

    function Modify(string description, uint256 price) public
    {
        if (State != StateType.Active)
        {
            revert("Invalid Action");
        }
        if (InstanceOwner != msg.sender)
        {
            revert("Invalid Action");
        }

        Description = description;
        AskingPrice = price;
    }

    function MakeOffer(address inspector, address appraiser, uint256 offerPrice) public
    {
        if (inspector == 0x0 || appraiser == 0x0 || offerPrice == 0)
        {
            revert("Invalid Action");
        }
        if (State != StateType.Active)
        {
            revert("Invalid Action");
        }
        // Cannot enforce "AllowedRoles":["Buyer"] because Role information is unavailable
        if (InstanceOwner == msg.sender) // not expressible in the current specification language
        {
            revert("Invalid Action");
        }

        InstanceBuyer = msg.sender;
        InstanceInspector = inspector;
        InstanceAppraiser = appraiser;
        OfferPrice = offerPrice;
        State = StateType.OfferPlaced;
    }

    function AcceptOffer() public
    {
        if (State != StateType.OfferPlaced)
        {
            revert("Invalid Action");
        }
        if (InstanceOwner != msg.sender)
        {
            revert("Invalid Action");
        }

        State = StateType.PendingInspection;
    }

    function Reject() public
    {
        if (State != StateType.OfferPlaced && State != StateType.PendingInspection && State != StateType.Inspected && State != StateType.Appraised && State != StateType.NotionalAcceptance && State != StateType.BuyerAccepted)
        {
            revert("Invalid Action");
        }
        if (InstanceOwner != msg.sender)
        {
            revert("Invalid Action");
        }

        InstanceBuyer = 0x0;
        State = StateType.Active;
    }

    function Accept() public
    {
        if (msg.sender != InstanceBuyer && msg.sender != InstanceOwner)
        {
            revert("Invalid Action");
        }

        if (msg.sender == InstanceOwner &&
            State != StateType.NotionalAcceptance &&
            State != StateType.BuyerAccepted)
        {
            revert("Invalid Action");
        }

        if (msg.sender == InstanceBuyer &&
            State != StateType.NotionalAcceptance &&
            State != StateType.SellerAccepted)
        {
            revert("Invalid Action");
        }

        if (msg.sender == InstanceBuyer)
        {
            if (State == StateType.NotionalAcceptance)
            {
                State = StateType.BuyerAccepted;
            }
            else if (State == StateType.SellerAccepted)
            {
                State = StateType.Accepted;
            }
        }
        else
        {
            if (State == StateType.NotionalAcceptance)
            {
                State = StateType.SellerAccepted;
            }
            else if (State == StateType.BuyerAccepted)
            {
                State = StateType.Accepted;
            }
        }
    }

    function ModifyOffer(uint256 offerPrice) public
    {
        if (State != StateType.OfferPlaced)
        {
            revert("Invalid Action");
        }
        if (InstanceBuyer != msg.sender || offerPrice == 0)
        {
            revert("Invalid Action");
        }

        OfferPrice = offerPrice;
    }

    function RescindOffer() public
    {
        if (State != StateType.OfferPlaced && State != StateType.PendingInspection && State != StateType.Inspected && State != StateType.Appraised && State != StateType.NotionalAcceptance && State != StateType.SellerAccepted)
        {
            revert("Invalid Action");
        }
        if (InstanceBuyer != msg.sender)
        {
            revert("Invalid Action");
        }

        InstanceBuyer = 0x0;
        OfferPrice = 0;
        State = StateType.Active;
    }

    function MarkAppraised() public
    {
        if (InstanceAppraiser != msg.sender)
        {
            revert("Invalid Action");
        }

        if (State == StateType.PendingInspection)
        {
            State = StateType.Appraised;
        }
        else if (State == StateType.Inspected)
        {
            State = StateType.NotionalAcceptance;
        }
        else
        {
            revert("Invalid Action");
        }
    }

    function MarkInspected() public
    {
        if (InstanceInspector != msg.sender)
        {
            revert("Invalid Action");
        }

        if (State == StateType.PendingInspection)
        {
            State = StateType.Inspected;
        }
        else if (State == StateType.Appraised)
        {
            State = StateType.NotionalAcceptance;
        }
        else
        {
            revert("Invalid Action");
        }
    }

    function GetStatus() public view returns(StateType)
    {
        return State;
    }

    function GetOfferPrice() public view returns(uint)
    {
        return OfferPrice;
    }

    function GetAskingPrice() public view returns(uint)
    {
        return AskingPrice;
    }
}