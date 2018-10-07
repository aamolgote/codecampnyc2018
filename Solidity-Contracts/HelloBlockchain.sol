// solium-disable linebreak-style
pragma solidity ^0.4.24;
contract HelloBlockchain {
     //Set of States
    enum StateType { Initialized, MessageSent}

    //List of properties
    StateType private _state;
    address private  _requestor;
    string private  _message;

    // constructor function
    constructor(string message) public{
        _requestor = msg.sender;
        _message = message;
        _state = StateType.Initialized;
    }

    function SendMessage(string message) public
    {
        _message = message;
        _state = StateType.MessageSent;
    }

    function GetStatus() public view returns(StateType)
    {
        return _state;
    }

    function GetMessage() public view returns(string)
    {
        return _message;
    }
}