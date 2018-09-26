export class ContractFunctionInfo {
    belongsToCOntractInstance: string;
    functionName: string;
    type: string;
    stateMutability: string;
    imputParamsList: InputParamsInfo[];
    transactionUser: string;
    readFunctionResponse: any;
    writeFunctionResponse: any;
    sequence: number;
}

export class InputParamsInfo {
    type: string;
    name: string;
    inputValue: any;
}

export class DeployedInstance {
    smartContractInstanceId: number;
    smartContractId: number;
    deployedAddress: string;
    initialData: string;
    deployByUserLoginId: string;
    deployedInstanceDisplayName: string;
    createdDatetime; Date;
    updatedDatetime: Date;
}

export class SmartContractInstance {
    smartContract: SmartContract;
    smartContractDeployedInstanceItems: DeployedInstance[];
}

export class DltBlock {
    blockNumber: number;
    transactionCount: number;
    blockDatetime: Date;
    blockHash: string;
    gasLimit: number;
    gasUsed: number;
    size: number;
    dltTRansactions: DltTransaction[];
    isCollapsed: boolean; 
}

export class DltTransaction {
    from: string;
    gas: number;
    gasPrice: number;
    input: string;
    nonce: number;
    to: number;
    value: number;
    transactionHash: string;
    smartContractTransactionId: number;
    smartConntractDeployedInstanceId: number;
    transactionUser: string;
    smartContractFunction: string;
    smartContractFunctionParameters: any;
    createdDatetime; Date;
    updatedDatetime: Date;
    smartContractName: string;
    smartContractFunctionParameterNames: SmartContractFunctionParamterInfo[];
}

export class SmartContractFunctionInfo {
    functionName: string;
    parameterNames: string[];
    parameterValues: string[];
}

export class SmartContractFunctionParamterInfo {
    paramName: string;
    paramValue: string;
}

export class ExecuteFunctionPayload {
    smartContractDeployedInstanceId: number;
    function: string;
    parameters: any[];
    transactionUser: string;
}

export class SmartContractTransaction {
    smartContractTransactionId: number;
    smartContractDeployedInstanceId: number;
    transactionHash: string;
    transactionUser: string;
    smartContractFunction: string;
    smartContractFunctionParameters: any[];
    smartContractFunctionParameterList: any[];
    createdDatetime; Date;
    updatedDatetime: Date;
    name: string;
    abi: string;
}

export class SmartContract {
    contractId: number;
    abi: string;
    byteCode: string;
    name: string;
    createdByUserLoginId: string;
    imagePath: string;
    createdDatetime; Date;
    updatedDatetime: Date;
    functions: SmartContractFunction[]
}

export class SmartContractFunction {
    smartContractFunctionId: number;
    smartContractId: number;
    functionName: string;
    functionType: string;
    sequence: number;
    createdDatetime; Date;
    updatedDatetime: Date;

}

export class SmartContractToBeDeployed {
    smartContractId: number;
    deployByUserLoginId: string;
    deploymentData: any[];
    deployedInstanceDisplayName: string;
}


export class UserDltAccount {
    userAccountId: number;
    userLoginId: string;
    accountAddress: string;
    passphrase: string;
    createdDatetime; Date;
    updatedDatetime: Date;
}